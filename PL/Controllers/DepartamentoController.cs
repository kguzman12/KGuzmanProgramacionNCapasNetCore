using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class DepartamentoController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result result = BL.Departamento.GetAll();
            ML.Departamento departamento = new ML.Departamento();

            if (result.Correct)
            {
                departamento.Departamentos = result.Objects;

            }
            else
            {
                ViewBag.Message = "Error al realizar la consulta";
            }

            return View(departamento);
        }

        [HttpGet]
        public ActionResult Form(int? idDepartamento)
        {
            ML.Departamento departamento = new ML.Departamento();
            departamento.Area = new ML.Area();

            ML.Result resultArea = BL.Area.GetAll();

            if (idDepartamento == null)
            {
                departamento.Area.Areas = resultArea.Objects;

                return View(departamento);
            }
            else
            {
                ML.Result result = BL.Departamento.GetById(idDepartamento.Value);

                if (result.Correct)
                {
                    departamento = (ML.Departamento)result.Object;
                    departamento.Area.Areas = resultArea.Objects;

                    return View(departamento);
                }
                else
                {
                    ViewBag.Message = "Error al consultar el departamento seleccionado";
                }
                return View(departamento);
            }

        }

        [HttpPost]
        public ActionResult Form(ML.Departamento departamento)
        {
            if (departamento.IdDepartamento == 0)
            {
                ML.Result result = BL.Departamento.Add(departamento);

                if (result.Correct)
                {
                    ViewBag.Message = result.Message;
                }
                else
                {
                    ViewBag.Message = "Error: " + result.Message;
                }
            }
            else
            {
                ML.Result result = BL.Departamento.Update(departamento);

                if (result.Correct)
                {
                    ViewBag.Message = result.Message;
                }
                else
                {
                    ViewBag.Message = "Error al actualizar el departamento";
                }
            }

            return PartialView("Modal");
        }

        [HttpGet]
        public ActionResult Delete(int idDepartamento)
        {
            if (idDepartamento >= 1)
            {
                ML.Result result = BL.Departamento.Delete(idDepartamento);

                if (result.Correct)
                {
                    ViewBag.Message = result.Message;
                }
                else
                {
                    ViewBag.Message = "Error al consultar el departamento seleccionado";
                }

            }
            return PartialView("Modal");

        }


    }
}
