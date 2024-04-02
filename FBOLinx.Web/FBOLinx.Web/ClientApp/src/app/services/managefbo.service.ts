import {  Injectable } from '@angular/core';
import { GroupFboViewModel } from '../models/groups';
import { foreignKeyData } from '@syncfusion/ej2-angular-grids';

@Injectable()
export class ManageFboGroupsService {
    public getGroupFbos(groupsFbosData: GroupFboViewModel,groupId: number) {
        return groupsFbosData.fbos.filter(
            (fbo) => fbo.groupId === groupId
        );
    }
    public isSingleSourceFbo(groupsFbosData: GroupFboViewModel,icao: string): boolean {
        return groupsFbosData.fbos.filter(f => f.icao == icao).length == 1;
    }
    public isNetworkFbo(groupsFbosData: GroupFboViewModel,groupId: number): boolean{
        var groupFbos =  groupsFbosData.groups.find(g => g.oid == groupId);
        return groupFbos.fbos.length > 1;
    }
}
