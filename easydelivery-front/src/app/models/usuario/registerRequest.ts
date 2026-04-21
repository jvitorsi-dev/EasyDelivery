import { UserRole } from "../enums/userRole";

export class RegisterRequest {
    email = '';
    senha = '';
    endereco?: string;
    nome = '';
    usuarioId?: number
    role!: UserRole 
}

