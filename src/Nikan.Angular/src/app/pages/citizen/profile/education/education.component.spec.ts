import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CitizenEducationComponent } from './education.component';

describe('CitizenEducationComponent', () => {
  let component: CitizenEducationComponent;
  let fixture: ComponentFixture<CitizenEducationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CitizenEducationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenEducationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
