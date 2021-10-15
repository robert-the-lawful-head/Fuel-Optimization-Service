export type EmailTemplate = {
    oid?: number;
    name?: string;
    subject: string;
    emailContentHtml: string;
    fboId?: number;
    groupId?: number;
    fromAddress?: string;
    replyTo?: string;
};
