import {
  trigger,
  transition,
  style,
  query,
  group,
  animate
} from '@angular/animations';

export const routeAnimations = trigger('routeAnimations', [
  transition('* <=> *', [
    query(':enter, :leave', [
      style({
        position: 'fixed',
        width: '100%'
      })
    ], { optional: true }),

    group([
      query(':leave', [
        animate('400ms cubic-bezier(0.4,0,0.2,1)', style({
          transform: 'translateX(-40px)',
          opacity: 0
        }))
      ], { optional: true }),

      query(':enter', [
        style({
          transform: 'translateX(40px)',
          opacity: 0
        }),
        animate('400ms cubic-bezier(0.4,0,0.2,1)', style({
          transform: 'translateX(0)',
          opacity: 1
        }))
      ], { optional: true })
    ])
  ])
]);