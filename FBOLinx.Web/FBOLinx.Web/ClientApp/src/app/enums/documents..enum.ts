export enum DocumentAcceptanceFlag {
    ForceAccepted = 0,
    Accepted = 1,
    NotAcceptedRequired = 2
}

export enum DocumentTypeEnum {
    EULA = 0,
    Cookie = 1,
    Privacy = 2
}

export const DocumentTypeEnumDescription: Record<DocumentTypeEnum, string> = {
    [DocumentTypeEnum.EULA]: "EULA",
    [DocumentTypeEnum.Cookie]: "Cookie Policy",
    [DocumentTypeEnum.Privacy]: "Privacy Policy"
  };
