import { AccountType } from "../enums/user-role";

export class User {
    oid: number;
    username: string;
    password: string;
    firstName: string;
    lastName: string;
    role: number;
    fboId: number;
    groupId: number;
    impersonatedRole: number;
    icao?: string;
    managerGroupId: number;
    conductorFbo?: boolean;
    isSingleSourceFbo: boolean;
    isNetworkFbo: boolean;
    accountType?: AccountType;
    isJetNetIntegrationEnabled?: boolean;
    decimalPrecision: number;
    remember: boolean;
    integrationStatus: boolean;
    authToken?: string;
}
