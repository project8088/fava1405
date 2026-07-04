import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CitizenEditProfileComponent } from './edit-profile.component';

describe('CitizenEditProfileComponent', () => {
  let component: CitizenEditProfileComponent;
  let fixture: ComponentFixture<CitizenEditProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CitizenEditProfileComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenEditProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
