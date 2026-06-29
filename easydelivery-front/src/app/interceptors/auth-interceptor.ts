import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth-service';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  const isPublicEndpoint = req.url.includes('/api/Auth') || req.url.includes('/api/Usuario/criar');

  if (isPublicEndpoint) {
    return next(req);
  }

  if (token) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });

    return next(cloned);
  }

  return next(req);
};
