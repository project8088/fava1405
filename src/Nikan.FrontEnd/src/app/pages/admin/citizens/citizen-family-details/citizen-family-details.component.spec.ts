import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CitizenFamilyDetailsComponent } from './citizen-family-details.component';

describe('CitizenFamilyDetailsComponent', () => {
  let component: CitizenFamilyDetailsComponent;
  let fixture: ComponentFixture<CitizenFamilyDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CitizenFamilyDetailsComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenFamilyDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
