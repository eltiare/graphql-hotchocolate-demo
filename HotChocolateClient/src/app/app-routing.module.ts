import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListComponent } from "./posts/list/list.component";
import { ShowComponent } from "./posts/show/show.component";
import { EditComponent } from "./posts/edit/edit.component";


const routes: Routes = [
  { path: '', component: ListComponent, pathMatch: 'full' },
  { path: 'edit', component: EditComponent },
  { path: 'show', component: ShowComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
