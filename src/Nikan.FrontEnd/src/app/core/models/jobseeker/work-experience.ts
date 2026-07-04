export class karjoWorkDto {
  public id?: number;

  /// <summary>
  /// عنوان شرکت
  /// </summary>
  public titleCompany: string;
  //------------------------------------------------------------------

  /// <summary>
  /// سمت
  /// </summary>
  public post: string;

  public city: string;
  public cityId: number;

  /// <summary>
  /// تاریخ شروع
  /// </summary>
  public dateOfStart?: string;

  /// <summary>
  /// تاریخ پایان
  /// </summary>
  public dateOfEnd?: string;

  public dateStart?: string;

  //------------------------------------------------------------------
  public dateEnd?: string;

  public userId?: string;

  /// <summary>
  /// دلیل قطع همکاری
  /// </summary>
  public cutCooperation: string;

  /// <summary>
  /// آیا بیمه رد میکرده ؟
  /// </summary>
  public isBimeh: boolean;

  /// <summary>
  /// شماره تماس شرکت
  /// </summary>
  public phoneNumber?: string;

  public loading?: boolean;
}
