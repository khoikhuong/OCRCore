using OCRCore.Common.Helper;
using System.Collections.Generic;
using System.Data;

namespace OCRCore.Business.Service
{
    public class TemplateService
    {
        public TemplateService()
        {
            
        }
        private const string usp_Template_Insert = "usp_Template_Insert";
        private const string usp_UpdateTemplate = "usp_UpdateTemplate";
        private const string usp_Region_Insert = "usp_Region_Insert";
        private const string usp_Template_Select = "usp_Template_Select";
        private const string usp_SelectRegionByTemplate = "usp_SelectRegionByTemplate";
        private const string usp_UpdateRegion = "usp_UpdateRegion";
        public const string usp_UpdateStatusTemplate = "usp_UpdateStatusTemplate";
        public const string usp_CheckTemplate = "usp_CheckTemplate";
        public const string usp_GetTemplateByID = "usp_GetTemplateByID";

        public int Create_Template(Dictionary<string, object> paramIN = null)
        {
            using (var baseRepository = new OCRDataContext())
            {
                return AppHelper.ToInt(baseRepository.ProcForScalar(usp_Template_Insert, paramIN));
            }
        }

        public int Update_Template(Dictionary<string, object> paramIN = null)
        {
            using (var baseRepository = new OCRDataContext())
            {
                return AppHelper.ToInt(baseRepository.ProcForScalar(usp_UpdateTemplate, paramIN));
            }
        }

        public int Create_Region(Dictionary<string, object> paramIN = null)
        {
            using (var baseRepository = new OCRDataContext())
            {
                return AppHelper.ToInt(baseRepository.ProcForScalar(usp_Region_Insert, paramIN));
            }
        }

        public DataTable Template_Select(Dictionary<string, object> paramIN = null)
        {
            using (var baseRepository = new OCRDataContext())
            {
                return baseRepository.ExecuteProcForDataTable(usp_Template_Select, paramIN);
            }
        }

        public DataTable Template_SelectById(Dictionary<string, object> paramIN = null)
        {
            using (var baseRepository = new OCRDataContext())
            {
                return baseRepository.ExecuteProcForDataTable(usp_GetTemplateByID, paramIN);
            }
        }

        public DataTable Region_SelectByTemplateID(Dictionary<string, object> paramIN = null)
        {
            using (var baseRepository = new OCRDataContext())
            {
                return baseRepository.ExecuteProcForDataTable(usp_SelectRegionByTemplate, paramIN);
            }
        }


        public int Update_Region(Dictionary<string, object> paramIN = null)
        {
            using (var baseRepository = new OCRDataContext())
            {
                return AppHelper.ToInt(baseRepository.ProcForScalar(usp_UpdateRegion, paramIN));
            }
        }

        public int Update_StatusTemplate(Dictionary<string, object> paramIN = null)
        {
            using (var baseRepository = new OCRDataContext())
            {
                return AppHelper.ToInt(baseRepository.ProcForScalar(usp_UpdateStatusTemplate, paramIN));
            }
        }

        public int CheckAndUpdateTemplate(Dictionary<string, object> paramIN = null)
        {
            using (var baseRepository = new OCRDataContext())
            {
                return AppHelper.ToInt(baseRepository.ProcForScalar(usp_CheckTemplate, paramIN));
            }
        }
    }
}