import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClienteHomePage } from './cliente-home-page';

describe('ClienteHomePage', () => {
  let component: ClienteHomePage;
  let fixture: ComponentFixture<ClienteHomePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClienteHomePage],
    }).compileComponents();

    fixture = TestBed.createComponent(ClienteHomePage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
