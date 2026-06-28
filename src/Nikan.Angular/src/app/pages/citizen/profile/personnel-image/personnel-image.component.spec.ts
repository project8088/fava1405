import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CitizenPersonnelImageComponent } from './personnel-image.component';

describe('CitizenPersonnelImageComponent', () => {
  let component: CitizenPersonnelImageComponent;
  let fixture: ComponentFixture<CitizenPersonnelImageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CitizenPersonnelImageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenPersonnelImageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
