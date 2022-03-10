

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using UserEvaluation.Data.Entities;
using UserEvaluation.Models;
using UserEvaluation.Services.Interfaces;

namespace UserEvaluation.Controllers
{
    public class EvaluationController : BaseController  
    {

        private readonly IRepository<Evaluation> _evaluationRepository;
        
        public EvaluationController(IRepository<Evaluation> evaluationRepository)
        {
            _evaluationRepository = evaluationRepository;
            
        }

        public IActionResult Add()
        {
            return View();
        }

        // GET: EvaluationController
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Add(UserRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
              
            int currentUserId = GetCurrentUserId();

            Evaluation entity = new Evaluation()
            {
                Name = model.Name,
                Surname = model.Surname,
                Description = model.Description,
                UserId = currentUserId,
                CreatedById = currentUserId,
                

            };

            #region File

            if (model.File.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    model.File.CopyTo(ms);
                    var fileByteArray = ms.ToArray();

                    entity.File = fileByteArray;
                }
            }
            else
            {
                ViewBag.Message = "Boş dosya yükleyemezsiniz";
            }

            #endregion

            bool result;
            entity.CreatedDate = DateTime.Now;
            result = _evaluationRepository.Add(entity);

            if (result)
            {
                return RedirectToAction("ListRequest");
            }

            ViewBag.Message = "Bir şeyler ters gitti!";
            return View("Add", model);
        }


        [AllowAnonymous]
        public ActionResult ListRequest(int id)
        {
            id = GetCurrentUserId();

            var requests = _evaluationRepository.GetAll(x => x.UserId == id && x.IsActive).Select(x => new UserRequestViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                FileStr = Convert.ToBase64String(x.File),
                AdminDescription = x.AdminMessage,
                IsPositive = x.IsPositive,
                UpdatedDate = x.UpdatedDate,
            }).ToList();

            return View(requests);
        }


        [Authorize(Roles = "2")]

        public IActionResult InProcess()
        {
            var requests = _evaluationRepository.GetAll().Where(x => x.IsEvaluated == false && x.IsActive).OrderBy(z=>z.CreatedDate).Select(x => new UserRequestViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                IsEvaluated = x.IsEvaluated,
                Description = x.AdminMessage,
                AdminDescription = x.Description,
                FileStr = Convert.ToBase64String(x.File),
                UserId = x.UserId,
                CreatedDate = x.CreatedDate,
            }).ToList();

            return View(requests);
        }


        [Authorize(Roles = "2")]
        public IActionResult EvaluationList()
        {
            var requests = _evaluationRepository.GetAll().Where(x => x.IsActive).OrderBy(z=>z.CreatedDate).Select(x => new UserRequestViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                IsEvaluated = x.IsEvaluated,
                Description = x.AdminMessage,
                AdminDescription = x.Description,
                FileStr = Convert.ToBase64String(x.File),
                UserId = x.UserId,
                CreatedDate = x.CreatedDate, // null gelmiyor ama listTheOld.cshtml'de null oluyor 
                UpdatedDate = x.UpdatedDate,
                IsPositive = x.IsPositive,

            }).ToList();

            return View(requests);
        }


        [Authorize(Roles = "2")]
        public IActionResult Report()
        {
            var requests = _evaluationRepository.GetAll().Where(x => x.IsActive).OrderBy(z => z.CreatedDate).Select(x => new UserRequestViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                IsEvaluated = x.IsEvaluated,
                Description = x.AdminMessage,
                AdminDescription = x.Description,
                FileStr = Convert.ToBase64String(x.File),
                UserId = x.UserId,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate,
                IsPositive = x.IsPositive,

            }).ToList();
            return View(requests);
        
        }


        [Authorize(Roles = "2")]

        public IActionResult Detail(int id)
        {
            var request = _evaluationRepository.Get(x => x.Id == id && x.IsActive);

            var vm = new UserRequestViewModel() 
            {
                Id = request.Id,
                Name = request.Name,
                Surname = request.Surname,
                Description = request.Description,
                FileStr = Convert.ToBase64String(request.File),
                IsPositive = request.IsPositive,
                CreatedDate = request.CreatedDate,
                UserId = request.UserId

            };

            return View(vm);
        }



        [HttpPost]
        [Authorize(Roles = "2")]
        public IActionResult Detail(UserRequestViewModel model)
        {

            var currentUserId = GetCurrentUserId();

            var entity = new Evaluation()
            {
                Name = model.Name,
                Surname = model.Surname,
                AdminMessage = model.AdminDescription,
                Description = model.AdminDescription,
                UserId = (int)model.UserId,
                IsActive = true,
            };

            bool result;

            byte[] file = Encoding.ASCII.GetBytes(model.FileStr); //
            entity.File = file;
            entity.Id = model.Id;
            entity.UpdatedById = currentUserId;
            entity.UpdatedDate = DateTime.Now;
            entity.IsEvaluated = true;
            entity.IsPositive = model.IsPositive;

            result = _evaluationRepository.Edit(entity);

            if (result)
            {
                return RedirectToAction("EvaluationList");
            }

            ViewBag.Message = "Bir şeyler ters gitti!";
            return View(model);
        }

       

    

        
    }
}
