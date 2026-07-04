import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS, MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
  MatNativeDateModule,
} from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MatSliderModule } from '@angular/material/slider';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatCardModule } from '@angular/material/card';
import { MatStepperModule } from '@angular/material/stepper';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule, MatPaginatorIntl } from '@angular/material/paginator';
import { getFarsiPaginatorIntl } from './paginator';
import { MAT_TABS_CONFIG, MatTabsModule } from '@angular/material/tabs';
import { MAT_DIALOG_DEFAULT_OPTIONS, MatDialogModule } from '@angular/material/dialog';
import { MatChipsModule } from '@angular/material/chips';
import {
  MAT_BUTTON_TOGGLE_DEFAULT_OPTIONS,
  MatButtonToggleModule,
} from '@angular/material/button-toggle';
import { MatListModule } from '@angular/material/list';
import {
  MAT_LUXON_DATE_FORMATS,
  LuxonDateAdapter,
  MAT_LUXON_DATE_ADAPTER_OPTIONS,
} from '@angular/material-luxon-adapter';
import { OVERLAY_DEFAULT_CONFIG } from '@angular/cdk/overlay';
import {
  MaterialJalaliDateAdapter,
  PERSIAN_DATE_FORMATS,
} from '@core/jalali/material-jalali-date-adapter';

export function getLanguage() {
  return 'fa-IR';
}
export function getDateFormat(locale: string) {
  if (locale === 'fa-IR') {
    return PERSIAN_DATE_FORMATS;
  } else {
    return MAT_LUXON_DATE_FORMATS;
  }
}
export function getDateProvider(locale: string) {
  if (locale === 'fa-IR') {
    return new MaterialJalaliDateAdapter(locale);
  } else {
    return new LuxonDateAdapter();
  }
}

const materialComponents = [
  MatInputModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatFormFieldModule,
  MatIconModule,
  MatButtonModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatCheckboxModule,
  MatSelectModule,
  DragDropModule,
  MatSliderModule,
  MatProgressBarModule,
  MatToolbarModule,
  MatMenuModule,
  MatSidenavModule,
  MatCardModule,
  MatStepperModule,
  MatAutocompleteModule,
  MatTooltipModule,
  MatTableModule,
  MatSortModule,
  MatPaginatorModule,
  MatTabsModule,
  MatDialogModule,
  MatChipsModule,
  MatButtonToggleModule,
  MatListModule,
];

@NgModule({
  declarations: [],
  imports: [CommonModule, materialComponents],
  exports: [materialComponents],
  providers: [
    { provide: MAT_LUXON_DATE_ADAPTER_OPTIONS, useValue: { useUtc: false } },
    { provide: MAT_DATE_LOCALE, useFactory: getLanguage, deps: [] },
    { provide: MAT_DATE_FORMATS, useFactory: getDateFormat, deps: [MAT_DATE_LOCALE] },
    { provide: DateAdapter, useFactory: getDateProvider, deps: [MAT_DATE_LOCALE] },
    {
      provide: MAT_DIALOG_DEFAULT_OPTIONS,
      useValue: {
        hasBackdrop: true,
        disableClose: true,
        panelClass: 'custom-dialog',
      },
    },
    {
      provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue: { appearance: 'outline' },
    },

    { provide: MAT_TABS_CONFIG, useValue: { stretchTabs: false } },
    {
      provide: MAT_BUTTON_TOGGLE_DEFAULT_OPTIONS,
      useValue: { hideMultipleSelectionIndicator: true, hideSingleSelectionIndicator: true },
    },

    // disable popover : Displaying dialogs as popovers causes the swal below them.
    {
      provide: OVERLAY_DEFAULT_CONFIG,
      useValue: {
        usePopover: false,
      },
    },

    { provide: MatPaginatorIntl, useClass: getFarsiPaginatorIntl },
  ],
})
export class MaterialModule {}
