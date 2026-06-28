import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminManzelatCitizensComponent } from './manzelat-citizens.component';

describe('AdminManzelatCitizensComponent', () => {
  let component: AdminManzelatCitizensComponent;
  let fixture: ComponentFixture<AdminManzelatCitizensComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminManzelatCitizensComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminManzelatCitizensComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
