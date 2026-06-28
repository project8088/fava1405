using Nikan.Common.GlobalEnum;
 

namespace Nikan.ViewModel
{
    public class FormulaDto
    {
        public int? Id { get; set; }
        /// <summary>
        /// عنوان فرمول
        /// </summary>
        public string Title { get; set; }
      
        /// <summary>
        /// فرمول
        /// </summary>
        public string StringFormula { get; set; }

        /// <summary>
        /// توضیحات فرمول
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// استفاده برای 
        /// </summary>
        public FormulaForEnum FormulaFor { get; set; }



        /// <summary>
        /// وضعیت دسترسی
        /// </summary>
        public bool IsActive { get; set; }






    }
}
