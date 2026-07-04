import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCitizenEditImageDialogComponent } from './citizen-edit-image.component';

describe('AdminCitizenEditImageDialogComponent', () => {
  let component: AdminCitizenEditImageDialogComponent;
  let fixture: ComponentFixture<AdminCitizenEditImageDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminCitizenEditImageDialogComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCitizenEditImageDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
