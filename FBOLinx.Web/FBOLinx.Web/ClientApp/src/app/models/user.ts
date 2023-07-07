import { AccountType } from "../enums/user-role";

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
    icao?: string;
    managerGroupId: number;
    conductorFbo?: boolean;
    accountType?: AccountType;
}
