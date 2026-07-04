import {
  FieldOfActivityEnum,
} from '../enums';

export class CompanyInfoDto {
  public companyId: number;

  // [StringLength(500)]
  public companyName: string;

  // [StringLength(40)]
  public englishName: string;

  // [StringLength(10)]
  public establishedYear: string; //  سال تاسیس

  // <summary>
  /// نوع فعالیت شرکت
  /// </summary>
  public fieldOfActivity: FieldOfActivityEnum;

  public managerName: string;

  // #region اطلاعات اصلی شرکت
  // [StringLength(40)]
  public slagUrl: string;

  public content: string;

  // [StringLength(50)]
  public cellNumber: string;
  public cellNumber2: string;
  public cellNumber3: string;
  // [StringLength(50)]
  public fax: string;

  // [StringLength(100)]
  public website: string;
  //  [StringLength(100)]
  public email: string;

  //  [StringLength(100)]
  public telegram: string;

  //  [StringLength(100)]
  public zipCode: string;

  //   [StringLength(100)]
  public street: string;

  //  [StringLength(1000)]
  public fullAddress: string;

  //  [StringLength(100)]
  public pelak: string;
  //   #endregion

  public imageUrl: string;
  public lat: string;
  public lng: string;

  public userCompanyActivities: any[];
  public products: any[] = [];
}
