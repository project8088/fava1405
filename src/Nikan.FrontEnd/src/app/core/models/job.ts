import { BaseDataModel } from './base-data-model';
export class QuickEmploymentInfoDto {
  public id?: number;

  public title: string;

  // public source?: string;
  public sex?: boolean;

  public number: number;

  public note: string;

  public jobRowCode: string;

  public isRetired?: boolean;
  public meal: boolean;
  public workService: boolean;
  public workServicePath: string;

  /// <summary>
  /// نوع حقوق پیشنهادی
  /// اداره کار-توافقی
  /// </summary>
  public salaryType: any;

  public workInsurance?: boolean;

  public workExperienceMin: number;
  public workExperienceMax: number;

  public skillIDs: number[];

  public majorIDs: number[];

  public cityIDs: number[];

  // public jobInfoIDs: number[];
  public jobTitleId: number;
  //public jobTitle: JobViewModel;

  public jobWorkTimeId?: number;

  public jobGrade: number;

  //public jobAge: number;

  public jobAgeStart?: number;
  public jobAgeEnd?: number;

  //  public  ExpireDate:string;

  // public  IsSpecial:boolean;

  public userCompanyId: number;
  public applicant: string;
  public applicantPost: string;
  public employerDescription: string;
  public applicantPhoneNumber: string;

  public similarJobTitles: string[];
  public similarJobTitle: string;

  public expireDay: number;

  public maritalStatus?: boolean;

  public militaryTrainingStatus?: boolean;

  public lackOfMedicalExemption?: boolean;

  //public hasEducation: boolean;

  public workTime: number;

  public salaryRangeFrom?: number;
  public salaryRangeTo?: number;

  // public placementDescription?: string;

  public locationId?: number;
  public location?: string;
  public jobFullAddress: string;
  public jobArea: number;
  public locationInfo?: BaseDataModel;

  public jobTitleStr?: string;

  //public IsLockEdit?: boolean;

  public cityInfos?: BaseDataModel;
  public majorInfos?: BaseDataModel;
  public skillInfos?: BaseDataModel;
}
