export interface AppLog {
  title: string;
  data: string;
  logColorCode: LogColorCode;
  logLevel: LogLevel;
}
export enum LogColorCode
{
    Red,
    Green,
    Blue,
    Yellow,
    Orange,
    Violet
}
export enum LogLevel
{
    Error,
    Warning,
    Info
}