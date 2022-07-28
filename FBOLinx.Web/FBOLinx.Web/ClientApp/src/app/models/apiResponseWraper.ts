export type ApiResponseWraper<T> = {
    success: boolean,
    message: string,
    result: T
};
