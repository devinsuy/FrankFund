import React from "react";

// reactstrap components
import { Breadcrumb, BreadcrumbItem } from "reactstrap";

export default class TableComponent extends React.Component {
    render() {
        return (
            <div>
                <Breadcrumb>
            <BreadcrumbItem active>Home</BreadcrumbItem>
          </Breadcrumb>
          <Breadcrumb>
            <BreadcrumbItem>
              <a href="#pablo" onClick={(e) => e.preventDefault()}>
                Home
              </a>
            </BreadcrumbItem>
            <BreadcrumbItem active>Library</BreadcrumbItem>
          </Breadcrumb>
          <Breadcrumb>
            <BreadcrumbItem>
              <a href="#pablo" onClick={(e) => e.preventDefault()}>
                Home
              </a>
            </BreadcrumbItem>
            <BreadcrumbItem>
              <a href="#pablo" onClick={(e) => e.preventDefault()}>
                Library
              </a>
            </BreadcrumbItem>
            <BreadcrumbItem active>Data</BreadcrumbItem>
          </Breadcrumb>
            </div>
            
        )
    }
}