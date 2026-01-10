# Validation

See also [RequiredValues](RequiredValues.md)

Validation cannot occur until the final values are determined. Since defaults may occur in the `MyArgs` via property initialization or the constructor finalizer, validation is done on the `MyArgs` instance, not the data values.

Property specific validation is done prior to custom validation, which is expected to validate the type. Custom validation is not done if any property validation fails.

## Stages

There are three stages of validation and stages are not executed if there is any failure in a preceding stage. This results in the user possibly having three sets of errors - where as soon as the last error in a stage is resolved, new errors appear. CLI entries are expected to be simple enough that this is preferable to sorting through meaningless messages that result because of an earlier stage failure.

* Required values
* Property validation
* Custom/type wide validation

## Attributes

Many validators will have an attribute to allow users to easily define them. At present, the name of the attribute class must be exactly _ValidatorTypeName_Attribute
