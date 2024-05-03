ARG GO_VERSION=1.22.1
ARG ALPINE_VERSION=3.18

FROM golang:${GO_VERSION}-alpine${ALPINE_VERSION}

# Install packages and Go language server
RUN apk add -q --update --progress --no-cache git sudo openssh-client bash make curl
RUN go install golang.org/x/tools/gopls@latest 2>&1 && \
    go install github.com/cweill/gotests/gotests@latest 2>&1 && \
    go install github.com/fatih/gomodifytags@latest 2>&1 && \
    go install github.com/josharian/impl@latest 2>&1 && \
    go install github.com/haya14busa/goplay/cmd/goplay@latest 2>&1 && \
    go install github.com/go-delve/delve/cmd/dlv@latest 2>&1 && \
    go install honnef.co/go/tools/cmd/staticcheck@latest 2>&1

# Install additional tools
RUN go install github.com/golangci/golangci-lint/cmd/golangci-lint@latest 2>&1