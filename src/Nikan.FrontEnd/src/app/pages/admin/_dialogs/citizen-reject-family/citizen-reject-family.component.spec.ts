import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCitizenRejectFamilyComponent } from './citizen-reject-family.component';

describe('AdminCitizenRejectFamilyComponent', () => {
  let component: AdminCitizenRejectFamilyComponent;
  let fixture: ComponentFixture<AdminCitizenRejectFamilyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCitizenRejectFamilyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCitizenRejectFamilyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
