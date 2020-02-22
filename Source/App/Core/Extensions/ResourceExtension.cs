using System.Collections.Generic;
using Project.Core.Enums;
using Project.Core.Enums.Framework;
using Project.Core.StaticResource.Models;

namespace Project.Core.Extensions
{
    public static class ResourceExtension
    {

        public static StaticResourceDto AddParent(this StaticResourceDto resource, StaticResourceDto parent)
        {
            resource.ParentState = parent.State;

            return resource;
        }


        public static StaticResourceDto AddChildren(this StaticResourceDto resource, StaticResourceDto children)
        {
            children.ParentState = resource.State;

            return children;
        }

        public static StaticResourceDto AddChildren(this StaticResourceDto resource, string displayName, string stateName)
        {
            var children = new StaticResourceDto(resource.ResourceOwner, $"{resource.Name}{displayName}", displayName, stateName, resource.Path, resource.IsPublic)
                { ParentState = resource.State };

            return children;
        }

        public static StaticResourceDto AddViewChildren(this StaticResourceDto resource, ResourceOwner? resourceOwner = null)
        {
            var view = new StaticResourceDto(resource.ResourceOwner, $"{resource.Name}View", "View", $"{resource.State}.view",
                    resource.Path, resource.IsPublic)
                { ParentState = resource.State };
            if (resourceOwner.HasValue) view.ResourceOwner = resourceOwner.GetValueOrDefault();

            return view;
        }

        public static StaticResourceDto AddCreateChildren(this StaticResourceDto resource, ResourceOwner? resourceOwner = null)
        {
            var create = new StaticResourceDto(resource.ResourceOwner, $"{resource.Name}Create", "Create",
                    $"{resource.State}.create", resource.Path, resource.IsPublic)
                { ParentState = resource.State };
            if (resourceOwner.HasValue) create.ResourceOwner = resourceOwner.GetValueOrDefault();

            return create;
        }

        public static StaticResourceDto AddEditChildren(this StaticResourceDto resource, ResourceOwner? resourceOwner = null)
        {
            var edit = new StaticResourceDto(resource.ResourceOwner, $"{resource.Name}Edit", "Edit",
                    $"{resource.State}.edit",resource.Path, resource.IsPublic){ ParentState = resource.State };
            if (resourceOwner.HasValue) edit.ResourceOwner = resourceOwner.GetValueOrDefault();


            return edit;
        }

        public static StaticResourceDto AddDeleteChildren(this StaticResourceDto resource, ResourceOwner? resourceOwner = null)
        {
            var delete = new StaticResourceDto(resource.ResourceOwner, $"{resource.Name}Delete", "Delete",
                    $"{resource.State}.delete", resource.Path, resource.IsPublic){ ParentState = resource.State };
            if (resourceOwner.HasValue) delete.ResourceOwner = resourceOwner.GetValueOrDefault();

            return delete;
        }


        public static List<StaticResourceDto> AddCrudChildren(this StaticResourceDto resource)
        {
            var view = new StaticResourceDto(resource.ResourceOwner, $"{resource.Name}View", "View", $"{resource.State}.view",
                resource.Path, resource.IsPublic) {ParentState = resource.State};

            var create = new StaticResourceDto(resource.ResourceOwner, $"{resource.Name}Create", "Create",
                $"{resource.State}.create", resource.Path, resource.IsPublic) {ParentState = resource.State};

            var edit = new StaticResourceDto(resource.ResourceOwner, $"{resource.Name}Edit", "Edit",
                $"{resource.State}.edit",
                resource.Path, resource.IsPublic) {ParentState = resource.State};

            var delete = new StaticResourceDto(resource.ResourceOwner, $"{resource.Name}Delete", "Delete",
                $"{resource.State}.delete", resource.Path, resource.IsPublic) {ParentState = resource.State};

            return new List<StaticResourceDto>()
            {
                view,
                create,
                edit,
                delete
            };
        }

    }
}