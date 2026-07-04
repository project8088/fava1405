export class KarjoGlobalInformationDto {
  public userId?: string;

  public userCode: string;

  public gender?: boolean;

  public mobileNumber: string;
  public nationalCode: string;
  public cellNumber: string;

  public email: string;
  public firstName: string;
  public lastName: string;
  public fatherName: string;

  public address: string;

  public cityId?: number;
  public city: string;

  public dateOfBirth?: string;

  public soldierState?: number;

  public marital?: number;

  public numberOfChildren?: number;

  public familiarName: string;

  public familiarCellNumber: string;

  public familiarMobile: string;

  public familiarOfficeCellNumber: string;

  public familiarRelationship: string;

  public familiarCode: string;

  public lastModifiedOnDate: string;
}

export class ShortKarjoProfile {
  public nationalCode: string;
  public personalPictureUrl?: string;

  public userName: string;

  public userCode: string;

  public gender: string;

  public firstName: string;

  public lastName: string;

  public percentageResume: number;

  public isMadadjo: boolean;
}
