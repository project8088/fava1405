import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCitizenSmsListDialogComponent } from './citizen-sms-list.component';

describe('AdminCitizenSmsListDialogComponent', () => {
  let component: AdminCitizenSmsListDialogComponent;
  let fixture: ComponentFixture<AdminCitizenSmsListDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminCitizenSmsListDialogComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCitizenSmsListDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
