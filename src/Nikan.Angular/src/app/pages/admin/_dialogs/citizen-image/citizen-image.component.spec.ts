import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCitizenImageDialogComponent } from './citizen-image.component';

describe('AdminCitizenImageDialogComponent', () => {
  let component: AdminCitizenImageDialogComponent;
  let fixture: ComponentFixture<AdminCitizenImageDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCitizenImageDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCitizenImageDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
