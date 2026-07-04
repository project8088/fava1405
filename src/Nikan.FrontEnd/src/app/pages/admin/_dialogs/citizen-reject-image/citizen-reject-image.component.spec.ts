import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCitizenRejectImageDialogComponent } from './citizen-reject-image.component';

describe('AdminCitizenRejectImageDialogComponent', () => {
  let component: AdminCitizenRejectImageDialogComponent;
  let fixture: ComponentFixture<AdminCitizenRejectImageDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCitizenRejectImageDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCitizenRejectImageDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
