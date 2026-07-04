export class karjoEducationDto {
  public gradeId: Grade;

  public grade?: string;

  public university?: string;

  public dateOfStart?: string;
  public dateOfEnd?: string;

  public id?: number;

  public majorId?: number;
  public major?: string;

  public userId?: string;

  public loading?: boolean;
}
export enum Grade {
  بیسواد = -10,
  ابتدايي = -9,
  راهنمايي = -8,
  متوسطه = -7,

  دیپلم = 0,
  فوق_دیپلم = 1,
  لیسانس = 2,
  فوق_لیسانس = 3,
  دکتری = 5,
}
