export class NewsDto {

  /// <summary>
  /// شناسه خبر
  /// </summary>
  public id?: any;



  /// <summary>
  /// عنوان خبر
  /// </summary>
  // [Required]
  // [StringLength(2000)]
  public title: string;


  /// <summary>
  /// توضیحات کوتاه
  /// </summary>
  public description: string;
  /// <summary>
  ///شرح کامل
  ///Html Editor
  /// </summary>
  public body: string;

  /// <summary>
  /// تعداد مشاهده خبر
  /// </summary>
  public clicks?: number;

  /// <summary>
  /// تاریخ انتشار خبر
  /// </summary>
  public publishDate?: any;


  /// <summary>
  /// تاریخ ایجاد خبر
  /// </summary>
  public onDate?: string;

  /// <summary>
  /// توضیحات سئو
  /// </summary>
  public seoDescription: string;


  /// <summary>
  /// تگهای سئو
  /// </summary>
  public seoTags: string;


  /// <summary>
  /// آیا امکان ارسال دیدگاه وجود دارد ؟
  /// </summary>
  public commentIsActive: boolean;


  public attachmentFileGroup?: string;
  public attachmentImageGroup?: string;
  public imageUrl?: string;



  public indexOrder?: number;


  public createdByUserId?: number;
  public createdByUser?: string;

  public newsGroupId?: number;
  public newsGroup?: string;


  /// <summary>
  /// مسیر دسترسی
  /// ویژه صفحات
  /// </summary>
  // [StringLength(200)]
  public slug?: string;


  // [StringLength(3)]
  public languageName?: string;

 

  /// <summary>
  /// آیا خبر ویژه است ؟
  /// </summary>
  public isSpecial: boolean;

  public organizationId?: string;
  public isDeleted?: boolean;
  public isActive?: boolean;
   

}

export class NewsCommentDto {
  public id?: number;

  public userIP?: string;

  public commentMessage: string;
  public emailAddress: string;

  public fullName: string;


  public replyId?: boolean;

  public isPublish?: boolean;
  public newsItemId: number;
}
