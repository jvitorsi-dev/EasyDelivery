import { UserRole } from "../enums/userRole";

export class UsuarioResponse {
    id!: number;
    nome!: string;
    email!: string;
    endereco?: string;
    roleId!: number;
    role!: UserRole;  
}