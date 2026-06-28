import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CitizenEditEmailComponent } from './edit-email.component';

describe('CitizenEditEmailComponent', () => {
  let component: CitizenEditEmailComponent;
  let fixture: ComponentFixture<CitizenEditEmailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CitizenEditEmailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenEditEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
