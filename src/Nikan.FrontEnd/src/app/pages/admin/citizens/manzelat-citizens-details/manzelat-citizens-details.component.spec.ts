import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminManzelatCitizensDetailsComponent } from './manzelat-citizens-details.component';

describe('AdminManzelatCitizensDetailsComponent', () => {
  let component: AdminManzelatCitizensDetailsComponent;
  let fixture: ComponentFixture<AdminManzelatCitizensDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminManzelatCitizensDetailsComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminManzelatCitizensDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
