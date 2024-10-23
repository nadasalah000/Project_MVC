
using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    //[Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            var MappedDepartment = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(MappedDepartment);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                var Mapped = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
               await _unitOfWork.DepartmentRepository.AddAsync(Mapped);
                int Result = await _unitOfWork.CompleteAsync();
                if(Result > 0)
                {
                    TempData["Message"] = "Department Is Created";
                }
                //var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                //_unitOfWork.DepartmentRepository.Add(MappedDepartment);
                //_unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }
        public async Task<IActionResult> Details(int? id, string ViewName="Details")
        {
            if (id is null)
                return BadRequest();
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if(department is null)
                return NotFound();
            var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(ViewName, MappedDepartment);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id) 
        {
            /*if (id is null)
                return BadRequest();
            var department = _departmentRepository.GetById(id.Value);
            if (department is null)
                return NotFound();
            return View(department);*/
            
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentViewModel departmentVM, [FromRoute] int id) 
        {
           if(id != departmentVM.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Update(MappedDepartment);
                   await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex) 
                {
                    ModelState.AddModelError(string.Empty,ex.Message);
                }
            }
            return View(departmentVM);
        }
        public async Task<IActionResult> Delete(int id) 
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(DepartmentViewModel departmentVM, [FromRoute] int id)
        { 
         if(id != departmentVM.Id)
                return BadRequest();
            try
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Delete(MappedDepartment);
               await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex) 
            { 
            ModelState.AddModelError(string.Empty, ex.Message);
                return View(departmentVM);
            }
        }
    }
}
