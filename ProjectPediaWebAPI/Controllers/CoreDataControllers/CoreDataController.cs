using System;
using System.Reflection;
using System.Web.Mvc;
using ProjectPediaWebAPI.PortfolioCore;

namespace ProjectPediaWebAPI.Controllers
{
    public class CoreDataController : DatabaseController
    {
        public ActionResult FetchData(string coreModuleName, string id)
        {
            bool useListMode = String.IsNullOrEmpty(id);
            switch (coreModuleName.ToLower())
            {
                case "project":
                    return useListMode ? GetList<Project_List>() : GetDetail<Project_FullItem>(id);

                case "collaborator":
                    return useListMode ? GetList<Collaborator_List>() : GetDetail<Collaborator_FullItem>(id);
                    
                case "skill":
                    return useListMode ? GetList<Skill_List>() : GetDetail<Skill_FullItem>(id);

                default:
                    return HttpNotFound();
            }
        }

        private ActionResult GetList<coreDataType>() where coreDataType : IPediaCore_List
        {
            coreDataType coreListModel = Activator.CreateInstance<coreDataType>();

            DatabaseResultCode dbResultCode = coreListModel.LoadFromDB(_ConnectionToDB);
            switch (dbResultCode)
            {
                case DatabaseResultCode.statusUnknown:
                case DatabaseResultCode.miscError:
                case DatabaseResultCode.notFound:
                    return HttpServerError();
                default:
                    break;
            }

            return Json(
                coreListModel,
                JsonRequestBehavior.AllowGet
            );
        }

        private ActionResult GetDetail<coreDataType>(string id) where coreDataType : IPediaCore_FullItem
        {
            Type[] argSignature = new Type[] { typeof(string) };
            ConstructorInfo ctor = typeof(coreDataType).GetConstructor(argSignature);

            object[] argValues = new object[] { id };
            coreDataType coreDetailModel = (coreDataType)ctor.Invoke(argValues);

            DatabaseResultCode dbResultCode = coreDetailModel.LoadFromDB(_ConnectionToDB);
            switch (dbResultCode)
            {
                case DatabaseResultCode.statusUnknown:
                case DatabaseResultCode.miscError:
                    return HttpServerError();
                case DatabaseResultCode.notFound:
                    return HttpNotFound();
                default:
                    break;
            }

            return Json(
                coreDetailModel,
                JsonRequestBehavior.AllowGet
            );
        }

        private HttpStatusCodeResult HttpServerError()
        {
            return new HttpStatusCodeResult(500);
        }

        private new HttpStatusCodeResult HttpNotFound()
        {
            return new HttpStatusCodeResult(404);
        }
    }

}
