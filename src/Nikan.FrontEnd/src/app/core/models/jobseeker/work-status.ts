export class UserWorkStatusDto {


    public id?: string;
    public currentTitleCompany: string;


    public insuranceNumber: string;

    /// <summary>
    /// تعداد سوابق کاری
    /// </summary>

    public workExperience: number;

    /// <summary>
    /// کار در خارج کشور
    /// </summary>

    public outsideTheCountry: boolean;


    /// <summary>
    /// خارج از محدوده
    /// </summary> 
    public outsideTheScope: boolean;

    /// <summary>
    /// وضعیت فعلی
    /// </summary>
    public currentWorkStatus: string;
    public currentWorkStatusId: number;


    /// <summary>
    /// بلند مدت
    /// </summary>
    public longTimeFullTime: boolean;
    /// <summary>
    /// کوتاه مدت
    /// </summary>
    public shortTimeFullTime: boolean;
    /// <summary>
    /// پاره وقت
    /// </summary>
    public partTime: boolean;
    /// <summary>
    /// پروژه ای
    /// </summary>
    public project: boolean;
    /// <summary>
    /// دور کاری
    /// </summary>
    public teleworking: boolean;
    /// <summary>
    /// کارآموز
    /// </summary>
    public karAmoze: boolean;



    /// <summary>
    /// متقاضی چه شعل هایی هستدید
    /// </summary>
    public job: string[];
    public jobIDs: number[];

    /// <summary>
    /// در چه شهرهایی تمایل به همکاری دارید
    /// </summary>
    public city: string[];
    public cityIDs: number[];

    public userId?: string;

    public cites: KeyValueItems[];
    public jobs: KeyValueItems[];

}

export class KeyValueItems {
    public id: number;
    public text: string;
}
