export enum UserRole {
    NotSet = 0,
    Primary = 1,
    GroupAdmin = 2,
    Conductor = 3,
    Member = 4,
    CSR = 5,
    NonRev = 6,
    X1 = 7
}
export enum AccountType {
    Premium = 0,
    Freemium = 1
}

//todo: evaluate if types are better over interface and change the code where types are used
export const accountStatusType: 'all' | 'active' | 'inactive' = 'all';
export const userAccountType: 'all' | 'premium' | 'freemium' = 'premium';