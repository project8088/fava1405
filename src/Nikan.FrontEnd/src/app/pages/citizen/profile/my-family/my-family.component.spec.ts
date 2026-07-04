import { ComponentFixture, TestBed, async } from '@angular/core/testing';

import { CitizenMyFamilyComponent } from './my-family.component';

describe('CitizenMyFamilyComponent', () => {
  let component: CitizenMyFamilyComponent;
  let fixture: ComponentFixture<CitizenMyFamilyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CitizenMyFamilyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenMyFamilyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
