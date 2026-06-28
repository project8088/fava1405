import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CitizenEditMobileComponent } from './edit-mobile.component';

describe('CitizenEditMobileComponent', () => {
  let component: CitizenEditMobileComponent;
  let fixture: ComponentFixture<CitizenEditMobileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CitizenEditMobileComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenEditMobileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
