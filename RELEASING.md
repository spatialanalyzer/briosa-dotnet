# Releasing NuGet clients

Each release publishes two independent exact-SA products. The current package
version is `0.3.0`, independent of the pinned Briosa Server `0.8.0` version.
This procedure does not establish licensed runtime or enterprise Artifactory
validation, which remain outstanding before v1.0 promotion.

## Registry setup

Set repository variable `NUGET_USER` to the individual NuGet login (`davidalucas`).
Create a NuGet trusted-publishing policy owned by `spatialanalyzer`, restricted to
GitHub repository `spatialanalyzer/briosa-dotnet`, workflow `release.yml`, and
environment `nuget`. Allow new packages and versions only for
`Briosa.2024.1.0508.5` and `Briosa.2026.1.0529.7`.

Create the GitHub environment `nuget`. Restrict its deployments to the branch
`main` (no tags), require the maintainer as reviewer, and turn off administrator
bypass. Trusted-publisher repository, workflow, and environment names must match
exactly and stay unchanged. Only the publishing job receives `id-token: write`, and
only the draft-release job receives `contents: write`; builds have read-only
permissions and never receive publishing credentials.

## Release procedure

Releases are published by dispatching `release.yml` on `main`, so only reviewed
code is published. The `v<version>` tag is created last, when the maintainer
publishes the draft GitHub release.

1. Update both targets' package versions together, review the PR, and merge with
   all CI checks passing. Keep target-specific package names and stable imports.
2. Optional dry run: dispatch `release.yml` on `main`, or on a branch, with
   `version=<version>` and `publish=false`. It runs the release checks and the
   complete CI matrix (protocol drift, conformance, tests, formatting, packaging,
   and installed-package smoke checks) and publishes nothing. Download the
   `packages-<SA target>` artifacts when needed for inspection.
3. Dispatch `release.yml` on `main` with `version=<version>` (without `v`) and
   `publish=true`. The workflow rejects tag refs, branches other than `main`, a
   version that differs from either target's package version, wrong target
   identities, and an existing `v<version>` tag at another commit. It rebuilds and
   validates both targets, then creates a draft GitHub release for `v<version>` at
   the run's commit with generated notes, before any registry write. The draft
   step fails if that tag's release is already published or a draft for it
   belongs to another commit; a draft for the same commit is reused. The run
   then waits for the `nuget` deployment approval.
4. The maintainer approves the deployment. Publishing downloads only artifacts from
   its own successful workflow run. No source build runs in the publishing job.
5. Verify installation from the actual registry in a clean consumer environment
   for each target. Check package version, exact SA target, and the stable public
   namespace/import. These checks must not start SpatialAnalyzer.
6. Complete the draft: link both registry packages and record the publishing run
   and clean-consumer verification results. Publishing the draft creates the
   `v<version>` tag at the release commit.

Agents never approve environment deployments or publish draft releases; the
maintainer does both.

## Partial failures

Registry versions are immutable. Publication is not atomic across the two
products. Inspect registry state after any failed or interrupted run before
retrying. Rerun only failed jobs when one target succeeded; a rerun of failed jobs
reuses the same commit, version, and draft release, and needs the deployment
approval again. Do not overwrite, delete, or silently skip an existing release. If
a published artifact needs a change, review and publish a new patch version. A
draft left by a run that published nothing can be deleted before dispatching again.

## If the draft release is refused

GitHub does not let the workflow token create a release whose commit has workflow
files that differ from `main`, for example when a workflow change merges while the
run waits. The draft step then fails before anything is published, and its job
summary repeats these steps. To release the approved commit unchanged:

1. Push the tag at the run's commit with your own credentials over SSH:
   `git push git@github.com:spatialanalyzer/briosa-dotnet.git <commit>:refs/tags/v<version>`.
   The release tag ruleset lets organization owners create it.
2. Use **Re-run failed jobs** on the same run. With the tag in place GitHub creates
   no tag, so the draft and publishing continue from the same commit and packages.
3. If the draft is still refused, create it on the Releases page from the existing
   tag, save it as a draft, and use **Re-run failed jobs** again. The draft step
   reuses a draft whose tag points at the run's commit.
