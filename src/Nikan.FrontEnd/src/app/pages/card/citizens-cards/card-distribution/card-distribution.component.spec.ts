import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCitizensComponent } from './citizens.component';

describe('AdminCitizensComponent', () => {
  let component: AdminCitizensComponent;
  let fixture: ComponentFixture<AdminCitizensComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminCitizensComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCitizensComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
