export class karjoAmozeshDto
{

    public id? :number;
    
    /// <summary>
    /// عنوان دوره آموزشی
    /// </summary>
     public  title :string;
     

    /// <summary>
    /// طول دوره به ساعت
    /// </summary>
    public  timeDure :string;
    //------------------------------------------------------------------
   /// <summary>
   /// چه سالی
   /// </summary>
    public  year :string;

    /// <summary>
    /// مرکز صادرکننده مدرک
    /// </summary>
    public  amozeshOrganzation :AmozeshOrganzationEnum; 

    /// <summary>
    /// نوع مدرک
    /// </summary>
    public  typeOfDocument :TypeOfDocumentEnum;


    /// <summary>
    /// آموزشگاه صادر کننده مدرک
    /// </summary>
    public   location :string;



    /// <summary>
    /// شهر صادر کننده مدرک تحصیلی
    /// </summary>
   public   city :string;
  public   cityId :number;

    public   userId? :string;
    public   loading? :boolean;





}



export enum AmozeshOrganzationEnum
{
    آموزشگاههای_آزاد,
    فنی_و_حرفه_ایی = 1,
    میراث_فرهنگی = 2,
    شهرداری = 3,
    کمیته_امداد = 4,
    سایر = 5,

}

export enum TypeOfDocumentEnum
{
    تاییده,
    گواهینامه

}

