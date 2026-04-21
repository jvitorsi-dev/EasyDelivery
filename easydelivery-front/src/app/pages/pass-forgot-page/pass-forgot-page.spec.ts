import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PassForgotPage } from './pass-forgot-page';

describe('PassForgotPage', () => {
  let component: PassForgotPage;
  let fixture: ComponentFixture<PassForgotPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PassForgotPage],
    }).compileComponents();

    fixture = TestBed.createComponent(PassForgotPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
