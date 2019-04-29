export class User {
    oid: number;
    username: string;
    password: string;
    firstName: string;
    lastName: string;
    token?: string;
    role: number;
    fboId: number;
    groupId: number;
    impersonatedRole: number;
}
