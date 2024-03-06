import {  Injectable } from '@angular/core';
import { GroupFboViewModel } from '../models/groups';

@Injectable()
export class ManageFboGroupsService {
    public getGroupFbos(groupsFbosData: GroupFboViewModel,groupId: number) {
        return groupsFbosData.fbos.filter(
            (fbo) => fbo.groupId === groupId
        );
    }
    public isSingleSourceFbo(groupsFbosData: GroupFboViewModel,groupId: number): boolean {
        return this.getGroupFbos(groupsFbosData,groupId).length == 1;
    }
    public isNetworkFbo(groupsFbosData: GroupFboViewModel,fbo: any): boolean{
        return groupsFbosData.groups.filter(
            (g) => g.fbos.filter(
                (f) => f.fbo == fbo.fbo
            ).length > 0
        ).length > 1;
    }
}
