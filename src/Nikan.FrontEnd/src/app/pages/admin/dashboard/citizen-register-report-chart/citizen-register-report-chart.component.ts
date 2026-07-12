import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription, finalize } from 'rxjs';
import { Chart } from 'angular-highcharts';
import { ServerApis } from '@core/server-apis';
import { FormGroup } from '@angular/forms';
import { AppBase } from '@app/app.base';

@Component({
  selector: 'adm-citizen-register-report-chart',
  templateUrl: './citizen-register-report-chart.component.html',
  styleUrls: ['./citizen-register-report-chart.component.scss'],
  standalone: false,
})
export class AdminDashboardCitizenRegisterReportChartComponent
  extends AppBase
  implements OnInit, OnDestroy
{
  loading?: boolean;
  report: any = {};
  subscribeReport?: Subscription;

  chart: Chart = new Chart();

  reportForm: FormGroup;

  constructor() {
    super();
    this.reportForm = this.fb.group({
      startDate: [null, []],
      endDate: [null, []],
    });
  }

  ngOnInit(): void {
    this.getReport();
  }

  ngOnDestroy(): void {
    if (this.subscribeReport) this.subscribeReport.unsubscribe();
  }

  getReport() {
    var formValue = this.reportForm.value;
    this.loading = true;
    this.subscribeReport = this.dataService
          .post(ServerApis.getCitizenRegisterChartReport, {
            StartDate: formValue.startDate ? this.dataService.formatDate(formValue.startDate) : '',
            EndDate: formValue.endDate ? this.dataService.formatDate(formValue.endDate) : '',
          })
    .pipe(
      finalize(() => {
        this.loading = false;
        this.chdr.detectChanges();
      }),
    )
    .subscribe((response) => {
              if (response.isSuccess) {
                this.report = response.data ? response.data : [];
                this.chart = this.createCharts(this.report.data, this.report.categories);
              } else {
                let msg = response.messages ? response.messages : 'متاسفانه خطایی در سرور رخ داده است!';
                this.toastrService.error(msg);
              }
            }, (error: any) => {
              this.toastrService.error('متاسفانه خطایی در سرور رخ داده است.');
            });
  }

  ngAfterViewInit() {}

  createCharts(dataSeries: any[], category: any[]) {
    return new Chart({
      credits: {
        enabled: false,
      },
      chart: {
        type: 'line',
      },
      title: {
        text: 'آمار ثبت نام در سامانه شهروندی',
      },
      subtitle: {
        text: '',
      },
      xAxis: {
        categories: category,
      },
      yAxis: {
        title: {
          text: 'تعداد .....',
        },
      },
      plotOptions: {
        line: {
          dataLabels: {
            enabled: true,
          },
          enableMouseTracking: false,
        },
      },
      series: [
        {
          type: undefined,
          name: '.......',
          data: dataSeries,
        },
      ],
      legend: {
        rtl: true,
      },
    });
  }
}
